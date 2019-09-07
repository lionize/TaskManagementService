Task Publish -Depends Pack {
   Exec { docker login docker.io --username=tiksn }
   $remoteTag = "docker.io/$script:latestImageTag"
   Exec { docker tag $script:latestImageTag $remoteTag }
   Exec { docker push $remoteTag }
}

Task Pack -Depends Build {
   $src = (Resolve-Path ".\src\").Path
   Exec { docker build -f Dockerfile $src -t $script:latestImageTag }
}

Task EstimateVersions {
   $script:VersionTags = @()

   if ($Latest) {
       $script:VersionTags += 'latest'
   }

   if (!!($Version)) {
       $Version = [Version]$Version

       Assert ($Version.Revision -eq -1) "Version should be formatted as Major.Minor.Patch like 1.2.3"
       Assert ($Version.Build -ne -1) "Version should be formatted as Major.Minor.Patch like 1.2.3"

       $Version = $Version.ToString()
       $script:VersionTags += $Version
   }

   Assert $script:VersionTags "No version parameter (latest or specific version) is passed."
}

Task Build -Depends TranspileModels {
   $script:publishFolder = Join-Path -Path $script:trashFolder -ChildPath "publish"

   New-Item -Path $script:publishFolder -ItemType Directory
   $project = Resolve-Path ".\src\TaskManagementService\TaskManagementService.csproj"
   $project = $project.Path
   Exec { dotnet publish $project --output $script:publishFolder }
}

Task TranspileModels -Depends Init,Clean {
   $apiModelYaml = (Resolve-Path ".\src\ApiModels.yml").Path
   $apiModelOutput = Join-Path -Path ".\src\TaskManagementService" -ChildPath "Models"
   Exec { smite --input-file $apiModelYaml --lang csharp --field property --output-folder $apiModelOutput }
}

Task Clean -Depends Init {
}

Task Init {
   $date = Get-Date
   $ticks = $date.Ticks
   $script:latestImageTag = "tiksn/lionize-task-management-service:latest"
   $trashFolder = Join-Path -Path . -ChildPath ".trash"
   $script:trashFolder = Join-Path -Path $trashFolder -ChildPath $ticks.ToString("D19")
   New-Item -Path $script:trashFolder -ItemType Directory
   $script:trashFolder = Resolve-Path -Path $script:trashFolder
}

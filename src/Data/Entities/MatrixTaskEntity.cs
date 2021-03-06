﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using TIKSN.Data;

namespace TIKSN.Lionize.TaskManagementService.Data.Entities
{
    public class MatrixTaskEntity : IEntity<string>
    {
        [BsonId]
        public string ID { get; set; }
        public Guid UserID { get; set; }
        public bool Completed { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? Important { get; set; }
        public bool? Urgent { get; set; }
        public int Order { get; set; }
    }
}

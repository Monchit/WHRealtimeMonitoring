﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WHRealtimeMonitoring.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class loginicsEntities : DbContext
    {
        public loginicsEntities()
            : base("name=loginicsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<tm_item_summary> tm_item_summary { get; set; }
        public DbSet<tm_user> tm_user { get; set; }
        public DbSet<tt_packing_order_nics> tt_packing_order_nics { get; set; }
        public DbSet<tt_picking_order_nics> tt_picking_order_nics { get; set; }
        public DbSet<tt_realtime_monitoring> tt_realtime_monitoring { get; set; }
        public DbSet<w_zone> w_zone { get; set; }
        public DbSet<tm_diagram_time_chart> tm_diagram_time_chart { get; set; }
    }
}

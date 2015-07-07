using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trirand.Web.Mvc;

namespace WHRealtimeMonitoring.Models
{
    public class OEMModel
    {
        public JQGrid OEMGrid { get; set; }

        public OEMModel()
        {
            OEMGrid = new JQGrid
            {
                Columns = new List<JQGridColumn>()
                {
                    new JQGridColumn { DataField = "process", HeaderText="PROCESS",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "trandate", HeaderText="DUE DATE",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "type", HeaderText="TYPE",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "tagno", HeaderText="TAGNO",
                                    Editable = true,
                                    Width = 100 },   
                    new JQGridColumn { DataField = "item", HeaderText="ITEM",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "qty", HeaderText="ORDER QTY",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "custno", HeaderText="CUST NO",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "custname", HeaderText="CUST NAME",
                                    Editable = true,
                                    Width = 100 },
                    //new JQGridColumn { DataField = "shipto", HeaderText="SHIP TO",
                    //                Editable = true,
                    //                Width = 100 },
                    //new JQGridColumn { DataField = "cycle", HeaderText="CYCLE NO",
                    //                Editable = true,
                    //                Width = 100 },
                    //new JQGridColumn { DataField = "zone", HeaderText="ZONE",
                    //                Editable = true,
                    //                Width = 100 },
                    new JQGridColumn { DataField = "status", HeaderText="STATUS",
                                    Editable = true,
                                    Width = 100 },
                    //new JQGridColumn { DataField = "shipvia", HeaderText="SHIP VIA",
                    //                Editable = true,
                    //                Width = 100 },
                    new JQGridColumn { DataField = "finish", HeaderText="FINISHED TIME",
                                    Editable =  true,
                                    Width = 100 }
                    //new JQGridColumn { DataField = "takeout_time",
                    //                Editable =  true,
                    //                Width = 100},
                    //new JQGridColumn { DataField = "takeout_user",
                    //                Editable =  true,
                    //                Width = 100},
                    //new JQGridColumn { DataField = "job_order_no", PrimaryKey = true,
                    //                Width = 100 }
                }
            };
        }
    }
}
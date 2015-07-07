using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trirand.Web.Mvc;

namespace WHRealtimeMonitoring.Models
{
    public class TimeChartExportModel
    {
        public JQGrid TimeChartGrid { get; set; }

        public TimeChartExportModel()
        {
            TimeChartGrid = new JQGrid
            {
                Columns = new List<JQGridColumn>()
                {
                    new JQGridColumn { DataField = "cust_no", HeaderText="CUST NO",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "process_no", HeaderText="PROCESS NO",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "start_time", HeaderText="START TIME",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "finished_time", HeaderText="FINISHED TIME",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "cycle_no", HeaderText="CYCLE NO",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "prepare_n_date", HeaderText="PREPARE DATE",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "shipto", HeaderText="SHIP TO",
                                    Editable = true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "username_id", HeaderText="UPDATE BY",
                                    Editable =  true,
                                    Width = 100 },
                    new JQGridColumn { DataField = "update_date", HeaderText="UPDATE DATE",
                                    Editable =  true,
                                    Width = 100},
                    new JQGridColumn { DataField = "update_time", HeaderText="UPDATE TIME",
                                    Editable =  true,
                                    Width = 100},
                    new JQGridColumn { DataField = "cust_type", HeaderText="CUST TYPE",
                                    Editable = true,
                                    Width = 100 }
                    //new JQGridColumn { DataField = "job_order_no", PrimaryKey = true,
                    //                Width = 100 }
                }
            };
        }
    }
}
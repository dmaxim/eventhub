using System;
using System.Collections.Generic;
using System.Text;

namespace K8sTestLogger.Model
{
    public class Envelope
    {

        public string SubmissionId { get; set; }

        public int EnvelopeId { get; set; }

        public string CreatedBy { get; set;  }

        public DateTime CreateDate { get; set; }
    }
}

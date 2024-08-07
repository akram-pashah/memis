using Microsoft.AspNetCore.Mvc.Rendering;

namespace MEMIS
{
  public class ListHelper
  {
    public static List<SelectListItem> ProjectType()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Construction", Value = "1" },
                new SelectListItem() { Text = "Work", Value = "2" },
                new SelectListItem() { Text = "Service", Value = "3" },
                new SelectListItem() { Text = "Programs", Value = "3" },
            };

      return results;
    }

    public static List<SelectListItem> ProductClassification()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Human", Value = "1" },
                new SelectListItem() { Text = "Vet", Value = "2" },
                new SelectListItem() { Text = "Other", Value = "3" },
            };

      return results;
    }


    public static List<SelectListItem> Months()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Jul", Value = "7" },
                new SelectListItem() { Text = "Aug", Value = "8" },
                new SelectListItem() { Text = "Sep", Value = "9" },
                new SelectListItem() { Text = "Oct", Value = "10" },
                new SelectListItem() { Text = "Nov", Value = "11" },
                new SelectListItem() { Text = "Dec", Value = "12" },
                new SelectListItem() { Text = "Jan", Value = "1" },
                new SelectListItem() { Text = "Feb", Value = "2" },
                new SelectListItem() { Text = "Mar", Value = "3" },
                new SelectListItem() { Text = "Apr", Value = "4" },
                new SelectListItem() { Text = "May", Value = "5" },
                new SelectListItem() { Text = "Jun", Value = "6" },
            };

      return results;
    }
    public static List<SelectListItem> NDPStatus()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Active", Value = "1" },
                new SelectListItem() { Text = "Not Active", Value = "0" },
            };
      return results;
    }
    public static List<SelectListItem> AchievementStatus()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Achieved", Value = "1.0" },
                new SelectListItem() { Text = "On Track", Value = "0.5" },
                new SelectListItem() { Text = "Not Achieved", Value = "0.25" },
            };

      return results;
    }

    public static List<SelectListItem> SDTAsessmentAction()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Verified", Value = "1" },
                new SelectListItem() { Text = "Rejected", Value = "0" },
            };

      return results;
    }

    public static List<SelectListItem> ApprovalStatus()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Approve", Value = "1" },
                new SelectListItem() { Text = "NotApproved", Value = "2" },
            };

      return results;
    }

    public static List<SelectListItem> FYear()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "FY1", Value = "1" },
                new SelectListItem() { Text = "FY2", Value = "2" },
                new SelectListItem() { Text = "FY3", Value = "3" },
                new SelectListItem() { Text = "FY4", Value = "4" },
                new SelectListItem() { Text = "FY5", Value = "5" },
            };

      return results;
    }

    public static List<SelectListItem> CategoryofPremises()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Retail", Value = "1" },
                new SelectListItem() { Text = "Wholesale", Value = "2" },
                new SelectListItem() { Text = "Medical", Value = "3" },
                new SelectListItem() { Text = "Device", Value = "4" },
                new SelectListItem() { Text = "AnnexStore", Value = "5" },
            };

      return results;
    }
    public static List<SelectListItem> UnregisteredDrugs()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Present", Value = "1" },
                new SelectListItem() { Text = "Not Present", Value = "2" },
            };

      return results;
    }
    public static List<SelectListItem> ClassofDrugs()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "A", Value = "1" },
                new SelectListItem() { Text = "B", Value = "2" },
                new SelectListItem() { Text = "C", Value = "3" },
            };

      return results;
    }
    public static List<SelectListItem> CategoryofDrugs()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Medical Device", Value = "1" },
                new SelectListItem() { Text = "Veterinary Drugs", Value = "2" },
                new SelectListItem() { Text = "Human Drugs", Value = "3" },
                new SelectListItem() { Text = "Public Healthcare Products", Value = "4" },
                new SelectListItem() { Text = "Herbal Drugs", Value = "5" },
            };

      return results;
    }
    public static List<SelectListItem> ComplianceAction()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Closed", Value = "1" },
                new SelectListItem() { Text = "Outlet abandoned by Owner", Value = "2" },
                new SelectListItem() { Text = "Impounded", Value = "3" },
                new SelectListItem() { Text = "Suspect Aarrested", Value = "4" },
                new SelectListItem() { Text = "No action Taken", Value = "5" },
            };

      return results;
    }
    public static List<SelectListItem> RecordKeeping()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Poor", Value = "1" },
                new SelectListItem() { Text = "Fair", Value = "2" },
                new SelectListItem() { Text = "Good", Value = "3" },
                new SelectListItem() { Text = "Excellent", Value = "4" },
            };

      return results;
    }
    public static List<SelectListItem> ConditionofPremises()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Poor", Value = "1" },
                new SelectListItem() { Text = "Fair", Value = "2" },
                new SelectListItem() { Text = "Good", Value = "3" },
                new SelectListItem() { Text = "Excellent", Value = "4" },
            };

      return results;
    }
    public static List<SelectListItem> CategoryofProducts()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "FPP", Value = "1" },
                new SelectListItem() { Text = "API", Value = "2" },
                new SelectListItem() { Text = "Herbal", Value = "3" },
                new SelectListItem() { Text = "Surgical Instruments", Value = "4" },
            };

      return results;
    }
    public static List<SelectListItem> FacilityStatus()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Open", Value = "1" },
                new SelectListItem() { Text = "Closed", Value = "2" },
            };

      return results;
    }
    public static List<SelectListItem> PersonFoundatFacility()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "In-charge", Value = "1" },
                new SelectListItem() { Text = "(Attendant/Operator)", Value = "2" },
            };

      return results;
    }
    public static List<SelectListItem> LicenseStatus()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Licensed", Value = "1" },
                new SelectListItem() { Text = "Un-Licensed", Value = "2" },
                 new SelectListItem() { Text = "Not- Applicable", Value = "3" },
            };

      return results;
    }
    public static List<SelectListItem> RiskRank()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Very Low", Value = "1" },
                new SelectListItem() { Text = "Low", Value = "2" },
                new SelectListItem() { Text = "Medium", Value = "3" },
                new SelectListItem() { Text = "High", Value = "4" },
                new SelectListItem() { Text = "Very High", Value = "5" },
            };
      return results;
    }
    public static List<SelectListItem> LicenseRecommendation()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Recommended For Licensing", Value = "1" },
                new SelectListItem() { Text = "Not Recommended for Licensing", Value = "2" },
            };

      return results;
    }
    public static List<SelectListItem> EnforcementAction()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Impound", Value = "1" },
                new SelectListItem() { Text = "Arrest", Value = "2" },
                new SelectListItem() { Text = "Close", Value = "3" },
                new SelectListItem() { Text = "Initiated Court Case", Value = "4" },
                new SelectListItem() { Text = "No Action Taken", Value = "5" },
                new SelectListItem() { Text = "Caution", Value = "6" },
                new SelectListItem() { Text = "Warning Letter", Value = "7" },
            };

      return results;
    }
    public static List<SelectListItem> FacilityType()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Public Facility", Value = "1" },
                new SelectListItem() { Text = "Private Facility", Value = "2" },
            };

      return results;
    }
    public static List<SelectListItem> CertificationStatus()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Certified", Value = "1" },
                new SelectListItem() { Text = "Not certified", Value = "2" },
            };

      return results;
    }
    public static List<SelectListItem> GPPRecommendation()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "GPP Certification", Value = "1" },
                new SelectListItem() { Text = "Not recommended for GPP", Value = "2" },
                 new SelectListItem() { Text = "Certification", Value = "3" },
            };

      return results;
    }
    public static List<SelectListItem> GDPRecommendation()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "GDP Certification", Value = "1" },
                new SelectListItem() { Text = "Not recommended for GDP", Value = "2" },
                 new SelectListItem() { Text = "Certification", Value = "3" },
            };

      return results;
    }
    public static List<SelectListItem> Impact()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Low", Value = "1" },
                new SelectListItem() { Text = "Medium", Value = "2" },
                new SelectListItem() { Text = "High", Value = "3" },
            };
      return results;
    }
    public static List<SelectListItem> Influence()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Low", Value = "1" },
                new SelectListItem() { Text = "Medium", Value = "2" },
                new SelectListItem() { Text = "High", Value = "3" },
            };
      return results;
    }
    public static List<SelectListItem> TypeofIndicator()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Impact Indicator", Value = "1" },
                new SelectListItem() { Text = "Outcome Indicator", Value = "2" },
                new SelectListItem() { Text = "Output Indicator", Value = "3" },
            };

      return results;
    }
    public static List<SelectListItem> Indicatorclassification()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Core Performance", Value = "1" },
                new SelectListItem() { Text = "KPI", Value = "2" },
            };

      return results;
    }
    public static List<SelectListItem> FrequencyofReporting()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Quarterly", Value = "1" },
                new SelectListItem() { Text = "Annually", Value = "2" },
                new SelectListItem() { Text = "5 Year", Value = "3" },
            };

      return results;
    }
    public static List<SelectListItem> Quarter()
    {
      List<SelectListItem> results = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Select Quarter", Value = "" },
                new SelectListItem() { Text = "Quarter 1", Value = "1" },
                new SelectListItem() { Text = "Quarter 2", Value = "2" },
                new SelectListItem() { Text = "Quarter 3", Value = "3" },
                new SelectListItem() { Text = "Quarter 4", Value = "4" }
            };

      return results;
    }

    public static string GetFiscalYearBackgroundColor(double value) =>
    value switch
    {
      < 0.67 => "red",
      < 0.9 => "yellow",
      _ => "lightgreen"
    };

    public static string GetFiscalYearColor(double value) =>
    value switch
    {
      < 0.67 => "white",
      < 0.9 => "black",
      _ => "black"
    };

  }
  public enum EnumProductClassification
  {
    Human = 1,
    Vet = 2,
    Other = 3
  }
  public enum EnumCategoryOfPremises
  {
    Retail = 1,
    Wholesale = 2,
    Medical = 3,
    Device = 4,
    AnnexStore = 5
  }
  public enum EnumApprovalStatus
  {
    Submit = 0,
    Approved = 1,
    NotApproved = 2
  }
  public enum EnumRiskRank
  {
    Very_Low = 1,
    Low_Value = 2,
    Medium = 3,
    High = 4,
    Very_High = 5
  }
  public enum riskIdentifyApprStatus
  {
    submitted = 0,
    hodreviewed = 1,
    hodrejected = 2,
    dirapprove = 3,
    dirrejected = 4,
    rmoapproved = 5,
    rmorejected = 6
  }
  public enum riskWorkFlowStatus
  {
    addedtoregister = 0,
    tolerence = 1,
    treatmentsubmitted = 2,
    treatmenthodreviewed = 3,
    treatmenthodrejected = 4,
    treatmentdirapprove = 5,
    treatmentdirrejected = 6,
    treatmentrmoapproved = 7,
    treatmentrmorejected = 8,
    monitoringsubmitted = 9,
    monitoringhodreviewed = 10,
    monitoringhodrejected = 11,
    monitoringdirapprove = 12,
    monitoringdirrejected = 13,
    monitoringrmoapproved = 14,
    monitoringrmorejected = 15,
    resdassesssubmitted = 16,
    resdassesshodreviewed = 17,
    resdassesshodrejected = 18,
    resdassessdirapprove = 19,
    resdassessdirrejected = 20,
    resdassessrmoapproved = 21,
    resdassessrmorejected = 22,
  }
  public enum EnumImpact
  {
    Low = 1,
    Medium = 2,
    High = 3,
  }
  public enum EnumInfluence
  {
    Low = 1,
    Medium = 2,
    High = 3,
  }
  public enum deptPlanApprStatus
  {
    submitted = 0,
    hodreviewed = 1,
    hodrejected = 2,
    dirapprove = 3,
    dirrejected = 4,
    meOfficerVerified = 5,
    meOfficerRejected = 6,
    headBpdVerified = 7,
    headBpdRejected = 8,
    dirapprapproved = 9,
    dirapprrejected = 10
  }
}

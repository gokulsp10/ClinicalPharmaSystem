namespace ClinicalPharmaSystem.Models
{
    public class DiseaseModel
    {
        public string? diseaseName { get; set; }
        public List<DiseaseTestModel>? DiseaseTests { get; set; }
    }

    public class DiseaseTestModel
    {
        public int DiseasesTestId { get; set; }
        public string? DiseasesTestName { get; set; }
        public int DiseasesId { get; set; }
        public string? Gender { get; set; }
        public decimal Ref_NormalStartValue { get; set; }
        public decimal Ref_NormalEndValue { get; set; }
        public decimal Ref_AboveNormalStartValue { get; set; }
        public decimal Ref_AboveNormalEndValue { get; set; }
        public decimal Ref_HighStartValue { get; set; }
        public decimal Ref_HighEndValue { get; set; }
        public decimal Ref_HigherStartValue { get; set; }
        public decimal Ref_HigherEndValue { get; set; }
        public DateTime EnteredDate { get; set; }
        public bool IsActive { get; set; }
        public int PatientId { get; set; }
        public string? DiseaseName { get; set; }
        public string? DiseaseTestValue { get; set; }
        public DateTime ReportDate { get; set; }
    }

}

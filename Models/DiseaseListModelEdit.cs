namespace ClinicalPharmaSystem.Models
{
    public class DiseaseListModelEdit
    {
        public int DiseasesTestId { get; set; }
        public string? DiseasesTestName { get; set; }
        public double Ref_NormalStartValue { get; set; }
        public double Ref_NormalEndValue { get; set; }
        public double Ref_AboveNormalStartValue { get; set; }
        public double Ref_AboveNormalEndValue { get; set; }
        public double Ref_HighStartValue { get; set; }
        public double Ref_HighEndValue { get; set; }
        public double Ref_HigherStartValue { get; set; }
        public double Ref_HigherEndValue { get; set; }
        public DateTime EnteredDate { get; set; }
        public bool IsActive { get; set; }
        public char Gender { get; set; }
    }
}
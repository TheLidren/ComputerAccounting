namespace ComputerAccounting.ListViewModel
{
    public enum ProviderState
    {
        SurnameAsc,
        SurnameDesc,
        OrganizationAsc,
        OrganizationDesc
    }

    public class ProviderSort
    {
        public ProviderSort(ProviderState providerState)
        {
            SurnameSort = providerState == ProviderState.SurnameAsc ? ProviderState.SurnameDesc : ProviderState.SurnameAsc;
            OrganizationSort = providerState == ProviderState.OrganizationAsc ? ProviderState.OrganizationDesc : ProviderState.OrganizationAsc;
        }

        public ProviderState SurnameSort { get; set; }
        public ProviderState OrganizationSort { get; set; }

    }
}

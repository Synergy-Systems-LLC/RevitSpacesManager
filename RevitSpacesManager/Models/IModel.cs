namespace RevitSpacesManager.Models
{
    internal interface IModel
    {
        RevitDocument RevitDocument { get; set; }

        void DeleteAll();

        void DeleteSelected(); 

        void CreateAll();

        void CreateSelected();
    }
}

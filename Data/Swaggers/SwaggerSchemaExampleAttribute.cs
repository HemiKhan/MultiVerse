namespace Data.Swaggers
{
    public class SwaggerSchemaExampleAttribute : Attribute
    {
        public string Example { get; set; }
        public SwaggerSchemaExampleAttribute(string example)
        {
            Example = example;
        }
    }
}

namespace manage_auth.src.models
{
    public class ResourceAction
    {
        public ResourceAction()
        {

        }
        
        public bool Get { get; set; }
        
        public bool Create { get; set; }
        
        public bool Update { get; set; }
        
        public bool Delete { get; set; }
    }
}
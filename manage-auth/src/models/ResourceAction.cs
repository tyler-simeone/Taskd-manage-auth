namespace manage_auth.src.models
{
    public class ResourceAction
    {
        public ResourceAction()
        {

        }
        
        public static bool Get { get; set; }
        
        public static bool Create { get; set; }
        
        public static bool Update { get; set; }
        
        public static bool Delete { get; set; }
    }
}
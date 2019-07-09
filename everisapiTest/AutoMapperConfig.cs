using AutoMapper;

namespace everisapiTest
{
    public class AutoMapperConfig
    {
        private readonly static AutoMapperConfig _instance = new AutoMapperConfig();
        private static MapperConfiguration MapperConfiguration { get; set; }
        private static IMapper Mapper { get; set; }
        
        private AutoMapperConfig()
        {
            MapperConfiguration = new MapperConfiguration(cfg => {cfg.AddProfile<AutomapperProfile>();});

            MapperConfiguration.AssertConfigurationIsValid();

            Mapper = MapperConfiguration.CreateMapper();
        }        

        public static AutoMapperConfig Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}

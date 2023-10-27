using AutoMapper;

namespace Auxiliar.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(configuration =>
            {
                // TODO adicionar aqui os profiles
                //configuration.AddProfile(new CommandToDomainMappingProfile());
            });
        }
    }
}
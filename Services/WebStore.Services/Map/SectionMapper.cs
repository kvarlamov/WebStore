using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
    public static class SectionMapper
    {
        public static SectionDto ToDTO(this Section section) => section is null ? null : new SectionDto
        {
            Id = section.Id,
            Name = section.Name
        };

        public static Section FromDTO(this SectionDto section) => section is null ? null : new Section
        {
            Id = section.Id,
            Name = section.Name
        };
    }
}
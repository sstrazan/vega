using AutoMapper;
using Vega.Controllers.Resources;
using Vega.Models;
using System.Linq;
using System.Collections.Generic;

namespace Vega.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap<Make, MakeResource>();
            CreateMap<Make, KeyValuePairResource>();
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Feature, KeyValuePairResource>();
            CreateMap<Vehicle, SaveVehicleResource>()
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource{ Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(f => f.FeatureId)));
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource{ Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                // .ForMember(vr => vr.Model, opt => opt.MapFrom(v => new ModelResource{Id = v.ModelId, Name = v.Model.Name})) // nije potrebno jer radi 'automatski'
                .ForMember(vr => vr.Make, opt => opt.MapFrom(v => v.Model.Make))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(f => new KeyValuePairResource{ Id = f.FeatureId, Name = f.Feature.Name})));

            // API Resouce to Domain
            CreateMap<SaveVehicleResource, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
                .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
                .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
                // .ForMember(v => v.Features, znj => znj.MapFrom(vr => vr.Features.Select(id => new VehicleFeature{ FeatureId = id })));  
                .ForMember(v => v.Features, opt => opt.Ignore())
                .AfterMap((vr, v) => {
                    // Remove unselected features
                    var removedFeatures = new List<VehicleFeature>();
                    foreach (var f in v.Features)
                    {
                        if (!vr.Features.Contains(f.FeatureId))
                        {
                            removedFeatures.Add(f);
                        }
                    }
                    foreach (var f in removedFeatures)
                    {
                        v.Features.Remove(f);
                    }
                    // ili ovako    
                    // var removedFeatures = v.Features.Where(f => !vr.Features.Contains(f.FeatureId));
                    // foreach (var f in removedFeatures)
                    //     v.Features.Remove(f);

                    // Add new features
                    // foreach (var id in vr.Features )
                    // {
                    //     if (!v.Features.Any(f => f.FeatureId == id))
                    //     {
                    //         v.Features.Add(new VehicleFeature{FeatureId = id });
                    //     }
                    // }
                    // ili ovako
                    var addedFeatures = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).Select(id => new VehicleFeature{FeatureId = id});    
                    foreach (var f in addedFeatures)
                        v.Features.Add(f);
                });  
        }
    }
}
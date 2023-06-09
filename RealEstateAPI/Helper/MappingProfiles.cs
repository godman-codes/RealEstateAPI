﻿using AutoMapper;
using RealEstateAPI.Dtos;
using RealEstateAPI.Model;

namespace RealEstateAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Listings, ListingResponseDto>();
            CreateMap<CreateListingDto, Listings >();
            CreateMap<OfferResponseDto, Offers>();
            CreateMap<Offers, OfferResponseDto>();
            CreateMap<Offers, RealtorsOfferResponseDto>();
            CreateMap<UsersOrRealtors, UserOrRealtorDto>();
            CreateMap<Images, ImageResponseDto>();
            CreateMap<ImageResponseDto, Images>();
        }
    }
}

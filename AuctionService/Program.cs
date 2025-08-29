using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(cfg => {
    cfg.CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
    cfg.CreateMap<Item, AuctionDto>();
    cfg.CreateMap<CreateAuctionDto, Auction>()
        .ForMember(d => d.Item, o => o.MapFrom(s => s));
    cfg.CreateMap<CreateAuctionDto, Item>();
    //CreateMap<AuctionDto, AuctionCreated>();
    //CreateMap<Auction, AuctionUpdated>().IncludeMembers(a => a.Item);
    //CreateMap<Item, AuctionUpdated>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

try
{
    DbInitializer.InitDb(app);
}
catch(Exception e)
{
    Console.WriteLine(e);
}

app.Run();
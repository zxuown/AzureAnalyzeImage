using Azure.Storage.Blobs;
using AzureAnalyzeImage.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureAnalyzeImage.Controllers;

[Route("home")]
[ApiController]
public class HomeController : ControllerBase
{
    private readonly BlobServiceClient _blobServiceClient;

    private readonly ComputerVisionService _computerVisionService;

    public HomeController(IConfiguration configuration, BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
        var subscriptionKey = configuration["AzureCognitiveServices:ComputerVision:SubscriptionKey"];
        var endpoint = configuration["AzureCognitiveServices:ComputerVision:Endpoint"];
        _computerVisionService = new ComputerVisionService(subscriptionKey, endpoint);
    }

    [HttpPost]
    public async Task<ActionResult> Index(IFormFile image)
    {
        var container = _blobServiceClient.GetBlobContainerClient("analyzeimages");
        await container.CreateIfNotExistsAsync();
        var client = container.GetBlobClient(image.FileName);
        await client.UploadAsync(image.OpenReadStream(), true);
        var serviceResult = await _computerVisionService.AnalyzeImageAsync(image.OpenReadStream());
        return Ok(new
        {
            Categories = serviceResult.Categories[0].Name,
            Description = serviceResult.Description.Captions[0].Text,
            Color = serviceResult.Color,
            IsAdultContent = serviceResult.Adult.IsAdultContent,
        });
        //return Ok(serviceResult);
    }
}

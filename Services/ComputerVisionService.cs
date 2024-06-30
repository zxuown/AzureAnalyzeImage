namespace AzureAnalyzeImage.Services;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class ComputerVisionService
{
    private readonly string subscriptionKey;
    private readonly string endpoint;
    private readonly ComputerVisionClient client;

    public ComputerVisionService(string subscriptionKey, string endpoint)
    {
        this.subscriptionKey = subscriptionKey;
        this.endpoint = endpoint;
        this.client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
        {
            Endpoint = endpoint
        };
    }

    public async Task<ImageAnalysis> AnalyzeImageAsync(Stream imageStream)
    {
        var features = new List<VisualFeatureTypes?>()
        {
            VisualFeatureTypes.Categories,
            VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces,
            VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags,
            VisualFeatureTypes.Color,
            VisualFeatureTypes.Adult
        };

        return await client.AnalyzeImageInStreamAsync(imageStream, features);
    }
}


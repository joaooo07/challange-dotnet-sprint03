using Microsoft.ML;
using System;
using System.Collections.Generic;

namespace ChallangeDotnet.ML.Sentiment
{
    public interface ISentimentService
    {
        SentimentResult Predict(string text);
    }

    public record SentimentResult(bool IsPositive, float Score, float Probability);

    public sealed class SentimentService : ISentimentService
    {
        private readonly MLContext _ml;
        private readonly PredictionEngine<SentimentData, SentimentPrediction> _predEngine;

        public SentimentService()
        {
            _ml = new MLContext(seed: 1);

            // Pequeno dataset de exemplo (PT+EN) só para a sprint
            var samples = new List<SentimentData>
            {
                new("I love this product", true),
                new("This is fantastic", true),
                new("not good", false),
                new("terrible experience", false),
                new("excelente serviço", true),
                new("muito bom", true),
                new("ótimo atendimento", true),
                new("péssimo atendimento", false),
                new("muito ruim", false),
                new("horrível", false),
            };

            var trainData = _ml.Data.LoadFromEnumerable(samples);

            var pipeline = _ml
                .Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
                .Append(_ml.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: nameof(SentimentData.Label),
                    featureColumnName: "Features"));

            var model = pipeline.Fit(trainData);

            _predEngine = _ml.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
        }

        public SentimentResult Predict(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new SentimentResult(false, 0f, 0f);

            var pred = _predEngine.Predict(new SentimentData(text, false));
            var prob = Sigmoid(pred.Score); // aproximação de probabilidade

            return new SentimentResult(pred.Prediction, pred.Score, prob);
        }

        private static float Sigmoid(float x) => 1f / (1f + MathF.Exp(-x));

        private sealed class SentimentData
        {
            public SentimentData() { }
            public SentimentData(string text, bool label)
            {
                Text = text;
                Label = label;
            }

            public string Text { get; set; } = string.Empty;
            public bool Label { get; set; }
        }

        private sealed class SentimentPrediction
        {
            public bool Prediction { get; set; }
            public float Score { get; set; }
        }
    }
}

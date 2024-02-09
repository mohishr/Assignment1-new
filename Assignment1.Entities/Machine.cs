using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Assignment1.Entities
{
    public class Machine
    {
        public string Type { get; set; }
        public List<Asset> Assets { get; set; }

        public Machine()
        {
        }

        public Machine(string type)
        {
            Type = type;
            Assets = new List<Asset>();
        }
        public Machine(string type, List<Asset> assets) : this(type)
        {
            Assets = assets;
        }
        public Machine(Machine other)
        {
            Type = other.Type;
            Assets = other.Assets.Select(asset => new Asset(asset)).ToList();
        }
        public void AddOrUpdateAsset(Asset asset)
        {
            var existingAsset = Assets.FirstOrDefault(a => a.Name == asset.Name);

            if (existingAsset != null)
            {
                existingAsset.SeriesNumber = asset.SeriesNumber;
            }
            else
            {
                Assets.Add(asset);
            }
        }
        public bool RemoveAsset(string assetName)
        {
            var assetToRemove = Assets.FirstOrDefault(a => a.Name == assetName);
            if (assetToRemove != null)
            {
                Assets.Remove(assetToRemove);
                return true;
            }
            return false;
        }
        public bool ContainsAsset(string assetName)
        {
            return Assets.Any(a => a.Name == assetName);
        }
        public IEnumerable<string> GetAssetNames()
        {
            return Assets.Select(a => a.Name);
        }
    }

}
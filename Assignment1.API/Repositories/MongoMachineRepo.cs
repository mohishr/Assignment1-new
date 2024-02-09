using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Runtime;
using Assignment1.API.Models;
using Assignment1.API.Settings;
using AutoMapper;
using MongoDB.Bson;

namespace Assignment1.API.Repositories
{
    public class MongoMachineRepo : IMachineRepo
    {
        private readonly IMongoCollection<MachineMongoModel> _machineCollection;
        private readonly IMapper _mapper;

        public MongoMachineRepo(IOptions<MongoDBSettings> settings, IMapper mapper)
        {
            var client = new MongoClient(settings.Value.connectionURI);
            var db = client.GetDatabase(settings.Value.databaseName);
            _machineCollection = db.GetCollection<MachineMongoModel>(settings.Value.collectionName);
            _mapper = mapper;
        }
        public async Task<IEnumerable<Machine>> GetMachinesByAssetAsync(Asset asset)
        {
            var filter = Builders<MachineMongoModel>.Filter.Where(machine =>
                machine.Assets.Any(machineAsset =>
                    asset.Name.Trim().ToLower()==machineAsset.Name.ToLower()&&asset.SeriesNumber.Trim().ToLower()==machineAsset.SeriesNumber.ToLower()
                )
            );
            var machines = await _machineCollection.Find(filter).Project(m=>new Machine(m.Type,m.Assets)).ToListAsync();
            return machines;
        }

        public async Task<IEnumerable<Machine>> GetAllMachinesAsync()
        {
            var machines = await _machineCollection.Find(m => true).Project(m => new Machine(m.Type,m.Assets)).ToListAsync();
            return machines;
        }

        public async Task<(bool Success, Machine ReplacedObject)> InsertOneAsync(Machine machine)
        {
            var machineMonogModel = _mapper.Map<MachineMongoModel>(machine);
            try
            {
                await _machineCollection.InsertOneAsync(machineMonogModel);
                return (true, machine);
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

        public async Task<Machine> GetByTypeAsync(string type)
        {
            var filter = Builders<MachineMongoModel>.Filter.Eq(m => m.Type, type);
            var res = await _machineCollection.Find(filter).Project(m => new Machine(m.Type,m.Assets)).FirstOrDefaultAsync();
            return res;
        }

        public async Task<(bool Success, Machine ReplacedObject)> ReplaceOneAsync(string type, Machine replacement)
        {
            var machineMongoModel = new MachineMongoModel();
            machineMongoModel.Type = replacement.Type;
            machineMongoModel.Assets = replacement.Assets;
            try
            {
                var result = await _machineCollection.Find(m=>m.Type==type).FirstOrDefaultAsync();
                var filter = Builders<MachineMongoModel>.Filter.Eq("_id",result.Id );

                machineMongoModel.Id = result.Id;
                if (result is not null)
                {

                    _machineCollection.ReplaceOneAsync(filter, machineMongoModel);
                    var t = _machineCollection.Find(m => true).ToListAsync();

                    return (true, replacement);
                }
                else
                {
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

        public async Task<bool> DeleteByTypeAsync(string type)
        {
            var filter = Builders<MachineMongoModel>.Filter.Eq(m => m.Type, type);
            var result = await _machineCollection.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}

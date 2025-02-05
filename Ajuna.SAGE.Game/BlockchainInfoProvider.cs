
namespace Ajuna.SAGE.Game
{
    public interface IBlockchainInfoProvider
    {
        byte[] GenerateRandomHash();
        uint CurrentBlockNumber { get; set; }
        void IncrementBlockNumber();
    }

    public class BlockchainInfoProvider : IBlockchainInfoProvider
    {
        private Random _random;

        public BlockchainInfoProvider(int seed)
        {
            _random = new Random(seed);
        }

        public byte[] GenerateRandomHash()
        {
            byte[] randomHash = new byte[32];
            _random.NextBytes(randomHash);
            return randomHash;
        }

        public uint CurrentBlockNumber { get; set; } = 1;

        public void IncrementBlockNumber()
        {
            CurrentBlockNumber++;
        }
    }
}
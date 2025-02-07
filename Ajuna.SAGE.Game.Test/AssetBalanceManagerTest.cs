using Ajuna.SAGE.Game.Manager;

namespace Ajuna.SAGE.Game.Test
{
    [TestFixture]
    public class AssetBalanceMangerTest
    {
        private BalanceManager _assetBalanceManager;

        [SetUp]
        public void Setup()
        {
            _assetBalanceManager = new BalanceManager();
        }

        [Test]
        public void Test_Deposit_NewAsset_AddsBalance()
        {
            ulong assetId = 1;
            uint depositAmount = 100;

            bool result = _assetBalanceManager.Deposit(assetId, depositAmount);

            Assert.That(result, Is.True, "Deposit should return true for a new asset.");
            Assert.That(_assetBalanceManager.AssetBalance(0), Is.EqualTo(null), "AssetBalance for ID 0 should be null.");
            Assert.That(_assetBalanceManager.AssetBalance(1), Is.EqualTo(100), "AssetBalance for ID 1 should be 100.");
            Assert.That(_assetBalanceManager.AllAssetBalances, Is.EqualTo(depositAmount), "Total balance should match the deposited amount.");
        }

        [Test]
        public void Test_Deposit_ExistingAsset_AccumulatesBalance()
        {
            ulong assetId = 1;
            uint initialDeposit = 100;
            uint additionalDeposit = 150;

            _assetBalanceManager.Deposit(assetId, initialDeposit);
            bool result = _assetBalanceManager.Deposit(assetId, additionalDeposit);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True, "Deposit should return true when accumulating balance.");
                Assert.That(_assetBalanceManager.AssetBalance(1), Is.EqualTo(initialDeposit + additionalDeposit), "AssetBalance for ID 1 should be the accumulated deposits.");
                Assert.That(_assetBalanceManager.AllAssetBalances, Is.EqualTo(initialDeposit + additionalDeposit), "Total balance should reflect the accumulated deposits.");
            });
        }

        [Test]
        public void Test_Deposit_ExceedsMaxValue_ReturnsFalse()
        {
            ulong assetId = 1;
            uint maxDeposit = uint.MaxValue;

            _assetBalanceManager.Deposit(assetId, maxDeposit);
            bool result = _assetBalanceManager.Deposit(assetId, 1);

            Assert.That(result, Is.False, "Deposit should return false if it exceeds uint.MaxValue.");
        }

        [Test]
        public void Test_Withdraw_ValidAmount_ReducesBalance()
        {
            ulong assetId = 1;
            uint initialDeposit = 200;
            uint withdrawAmount = 100;

            _assetBalanceManager.Deposit(assetId, initialDeposit);
            bool result = _assetBalanceManager.Withdraw(assetId, withdrawAmount);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True, "Withdraw should return true for valid amounts.");
                Assert.That(_assetBalanceManager.AssetBalance(1), Is.EqualTo(initialDeposit - withdrawAmount), "AssetBalance for ID 1 should be reduced by the withdrawn amount.");
                Assert.That(_assetBalanceManager.AllAssetBalances, Is.EqualTo(initialDeposit - withdrawAmount), "Total balance should be reduced by the withdrawn amount.");
            });
        }

        [Test]
        public void Test_Withdraw_ExceedsBalance_ReturnsFalse()
        {
            ulong assetId = 1;
            uint initialDeposit = 100;
            uint withdrawAmount = 200;

            _assetBalanceManager.Deposit(assetId, initialDeposit);
            bool result = _assetBalanceManager.Withdraw(assetId, withdrawAmount);

            Assert.That(result, Is.False, "Withdraw should return false if withdrawal exceeds balance.");
        }

        [Test]
        public void Test_Withdraw_NonExistentAsset_ReturnsFalse()
        {
            ulong assetId = 1;
            uint withdrawAmount = 50;

            bool result = _assetBalanceManager.Withdraw(assetId, withdrawAmount);

            Assert.That(result, Is.False, "Withdraw should return false for non-existent asset.");
        }
    }
}
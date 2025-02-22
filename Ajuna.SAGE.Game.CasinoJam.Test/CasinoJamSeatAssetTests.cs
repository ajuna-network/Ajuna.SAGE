using Ajuna.SAGE.Game.CasinoJam;
using Ajuna.SAGE.Game.CasinoJam.Model;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamSeatAssetTests
    {
        [Test]
        public void Test_SeatAssetProperties()
        {
            // Create a new SeatAsset instance using the genesis value (e.g., 1)
            var seatAsset = new SeatAsset(0, 1);

            // Test SeatValidityPeriod (2 bytes, stored at offset 4)
            RentDuration rentDuration = RentDuration.Day1;
            seatAsset.RentDuration = rentDuration;
            Assert.That(seatAsset.RentDuration, Is.EqualTo(rentDuration));

            // Test PlayerGracePeriod (1 byte, stored at offset 11)
            byte gracePeriod = 15;
            seatAsset.PlayerGracePeriod = gracePeriod;
            Assert.That(seatAsset.PlayerGracePeriod, Is.EqualTo(gracePeriod));

            // Test ReservationStartBlock (4 bytes, stored at offset 12)
            uint reservationStartBlock = 123456;
            seatAsset.ReservationStartBlock = reservationStartBlock;
            Assert.That(seatAsset.ReservationStartBlock, Is.EqualTo(reservationStartBlock));

            // Test ReservationDuration (2 bytes, stored at offset 16)
            ReservationDuration reservationDuration = ReservationDuration.Mins5;
            seatAsset.ReservationDuration = reservationDuration;
            Assert.That(seatAsset.ReservationDuration, Is.EqualTo(reservationDuration));

            // Test LastActionBlock (2 bytes, stored at offset 20)
            ushort lastActionBlock = 999;
            seatAsset.LastActionBlock = lastActionBlock;
            Assert.That(seatAsset.LastActionBlock, Is.EqualTo(lastActionBlock));

            // Test PlayerActionCount (2 bytes, stored at offset 22)
            ushort actionCount = 10;
            seatAsset.PlayerActionCount = actionCount;
            Assert.That(seatAsset.PlayerActionCount, Is.EqualTo(actionCount));

            // Test PlayerId (4 bytes, stored at offset 24)
            uint playerId = 123;
            seatAsset.PlayerId = playerId;
            Assert.That(seatAsset.PlayerId, Is.EqualTo(playerId));

            // Test MachineId (4 bytes, stored at offset 28)
            uint machineId = 456;
            seatAsset.MachineId = machineId;
            Assert.That(seatAsset.MachineId, Is.EqualTo(machineId));
        }

    }
}
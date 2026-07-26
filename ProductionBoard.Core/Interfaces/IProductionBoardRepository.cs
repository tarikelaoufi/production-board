using System;
using BoardModel = ProductionBoard.Core.Models.ProductionBoard;
using ProductionBoard.Core.Models;

namespace ProductionBoard.Core.Interfaces
{
    public interface IProductionBoardRepository
    {
        BoardModel GetById(int boardId);

        BoardModel Find(
            DateTime boardDate,
            string teamName,
            string shiftName,
            string lineName,
            string productName);

        int Create(BoardModel board);

        void SaveHour(ProductionHour hour);
    }
}
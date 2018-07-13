using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using _2048.Core.Model;

namespace _2048.Core.ViewModels
{
    public class GameViewModel : MvxViewModel
    {
        private GameModel _game;
        private bool _moveInProgress;

        public GameViewModel()
        {
            _game = new GameModel(4, 4);
        }

        public Cell[][] GameField => _game.Cells;
        public int Score => _game.Score;

        private MvxCommand _moveCommand;

        public ICommand MoveCommand(MoveDirection move)
        {
            _moveCommand = _moveCommand ?? new MvxCommand(() => HandleMove(move));
            return _moveCommand;
        }

        private void HandleMove(MoveDirection direction)
        {
            if (_moveInProgress)
            {
                return;
            }

            _moveInProgress = true;

            if (_game.PerformMove(direction))
            {
                RaiseAllPropertiesChanged();
            }
            else
            {
                _moveInProgress = false;
            }
        }
    }
}
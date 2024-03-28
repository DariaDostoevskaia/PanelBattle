using LegoBattaleRoyal.Infrastructure.Repository;
using LegoBattaleRoyal.Presentation.UI.LevelSelect;
using System.Linq;

namespace LegoBattaleRoyal.Presentation.Controllers.LevelSelect
{
    public class LevelSelectController
    {
        private readonly LevelSelectView _levelSelectView;
        private readonly LevelRepository _levelRepository;

        public LevelSelectController(LevelSelectView levelSelectView, LevelRepository levelRepository)
        {
            _levelSelectView = levelSelectView;
            _levelRepository = levelRepository;
        }

        public void ShowLevelSelect()
        {
            _levelSelectView.SetLevels(_levelRepository.GetAll().ToArray());
        }

        public void CloseLevelSelect()
        {
            _levelSelectView.Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProj
{
    public class GamesDetailList
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public string Column6 { get; set; }
        public string Column7 { get; set; }
        public string Column8 { get; set; }
        public string Column9 { get; set; }
        public string Column10 { get; set; }
        public string Column11 { get; set; }
        public string Column12 { get; set; }


    }

    public  class GamesList
    {
        private string _gamesName;
        public string GameName
        {
            get
            {
                return _gamesName;
            }
            set
            {
                _gamesName = value;
            }
        }

        private string _gameCode;
        public string GameCode
        {
            get
            {
                return _gameCode;
            }
            set
            {
                _gameCode = value;
            }
        }

        private List<string> _gameModes;
        public List<string> GameModes
        {
            get
            {
                return _gameModes;
            }
            set
            {
                _gameModes = value;
            }
        }

        private List<string> _regions;
        public List<string> Regions
        {
            get
            {
                return _regions;
            }
            set
            {
                _regions = value;
            }
        }
    }
}

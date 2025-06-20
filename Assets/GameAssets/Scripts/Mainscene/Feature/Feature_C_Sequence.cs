using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Feature_C_Sequence : MonoBehaviour
{

    public List<List<GridInfo>> AllSpinSequences = new List<List<GridInfo>>();
    public List<List<GridInfo>> AllRefillSequences = new List<List<GridInfo>>();

    private void Awake ()
    {
        collectAllSpinSequences();
        collectAllRefillSequences();
    }

    void collectAllSpinSequences ()
    {
        AllSpinSequences = this.GetType()
            .GetFields()
            .Where(f => f.Name.StartsWith("BG_Spin_") || f.Name.StartsWith("FG_Spin_"))
            .OrderBy(f => int.Parse(f.Name.Split('_').Last()))
            .Select(f => (List<GridInfo>)f.GetValue(this))
            .ToList();
    }
    void collectAllRefillSequences ()
    {
        AllRefillSequences = this.GetType()
            .GetFields()
            .Where(f => f.Name.StartsWith("FG_refill_"))
            .OrderBy(f => int.Parse(f.Name.Split('_').Last()))
            .Select(f => (List<GridInfo>)f.GetValue(this))
            .ToList();
    }

    public List<GridInfo> BG_Spin_1 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_2 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_1 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SUPER_JOKER.ToString(), isGolden = false},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_3 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false},
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false},
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_4 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_2 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.SUPER_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_3 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true},
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_4 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true},
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_5 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true},
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_6 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false},
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true},
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_7 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false},
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true},
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_5 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = true},
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_8 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SUPER_JOKER.ToString(), isGolden = false},
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_6 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true},
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false},
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_9 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SUPER_JOKER.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false},
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_10 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false},
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_11 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_12 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_13 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_14 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_15 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false},
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_7 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true},
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false},
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_16 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true},
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SUPER_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_17 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true},
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_8 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_18 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
                new CardDatas { name = CardType.BIG_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false,substitute = CardType.BIG_JOKER.ToString() },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false ,substitute = CardType.BIG_JOKER.ToString()},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false,substitute = CardType.BIG_JOKER.ToString()},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_19 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true},
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_20 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false},
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_21 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = true},
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false},
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_9 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_22 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SUPER_JOKER.ToString(), isGolden = false},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_10 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_23 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SUPER_JOKER.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_24 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_25 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_11 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_26 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SUPER_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_27 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_12 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_28 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.BIG_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false ,substitute = CardType.BIG_JOKER.ToString()},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false ,substitute = CardType.BIG_JOKER.ToString()},
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false ,substitute = CardType.BIG_JOKER.ToString()},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false ,substitute = CardType.BIG_JOKER.ToString()},
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_29 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_30 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_31 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = true },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.ACE.ToString(), isGolden = true },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_32 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_33 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = true },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_13 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.SCATTER.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_14 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_34 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = true },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_35 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SMALL_JOKER.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false},
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = true },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_15 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false},
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_refill_36 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = true },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false},
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        }
    };

    public List<GridInfo> FG_Spin_16 = new List<GridInfo>()
    {
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.SPADE.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.ACE.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.QUEEN.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false},
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = true },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.DIAMOND.ToString(), isGolden = false },
            }
        },
        new GridInfo
        {
            List = new List<CardDatas>()
            {
                new CardDatas { name = CardType.KING.ToString(), isGolden = false },
                new CardDatas { name = CardType.CLUB.ToString(), isGolden = false },
                new CardDatas { name = CardType.JACK.ToString(), isGolden = false },
                new CardDatas { name = CardType.HEART.ToString(), isGolden = false },
            }
        }
    };
}

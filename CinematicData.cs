// Decompiled with JetBrains decompiler
// Type: CinematicData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "New Cinematic", menuName = "Cinematic Data", order = 62)]
public class CinematicData : ScriptableObject
{
  [SerializeField]
  private string cinematicId;
  [SerializeField]
  private GameObject cinematicGo;
  [SerializeField]
  private AudioClip cinematicBSO;
  [SerializeField]
  private EventData cinematicEvent;
  [SerializeField]
  private CombatData cinematicCombat;
  [SerializeField]
  private bool cinematicEndAdventure;

  public string CinematicId
  {
    get => this.cinematicId;
    set => this.cinematicId = value;
  }

  public GameObject CinematicGo
  {
    get => this.cinematicGo;
    set => this.cinematicGo = value;
  }

  public EventData CinematicEvent
  {
    get => this.cinematicEvent;
    set => this.cinematicEvent = value;
  }

  public CombatData CinematicCombat
  {
    get => this.cinematicCombat;
    set => this.cinematicCombat = value;
  }

  public bool CinematicEndAdventure
  {
    get => this.cinematicEndAdventure;
    set => this.cinematicEndAdventure = value;
  }

  public AudioClip CinematicBSO
  {
    get => this.cinematicBSO;
    set => this.cinematicBSO = value;
  }
}

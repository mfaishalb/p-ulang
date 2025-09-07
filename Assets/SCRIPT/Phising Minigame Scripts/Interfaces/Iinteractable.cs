using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface Iinteractable
{
    public string InteractionPrompt { get; }

    public bool Interact(Interactor interactor);
}

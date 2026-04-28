using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

// Discrete time-of-day system using named slots instead of a clock.
// Attach this to a persistent manager GameObject.
public class TimeOfDay : MonoBehaviour
{
    public TextMeshProUGUI textTime;

    // -----------------------------------------------------------------------
    // Enums & Data
    // -----------------------------------------------------------------------

    public enum TimeSlot
    {
        Morning,    // sunrise, NPCs wake up, shops open
        Afternoon,  // midday activity
        Evening,    // winding down, some shops close
        Night       // dark, fewer NPCs, different enemies
    }

    [Serializable]
    public class TimeSlotSettings
    {
        public TimeSlot slot;
        [Tooltip("How many real-time seconds this slot lasts.")]
        public float durationSeconds = 120f;
    }

    // -----------------------------------------------------------------------
    // Inspector Fields
    // -----------------------------------------------------------------------

    [Header("Time Slots")]
    [Tooltip("Configure duration for each time slot. Order matters.")]
    public TimeSlotSettings[] slots = new TimeSlotSettings[]
    {
        new TimeSlotSettings { slot = TimeSlot.Morning,   durationSeconds = 120f },
        new TimeSlotSettings { slot = TimeSlot.Afternoon, durationSeconds = 180f },
        new TimeSlotSettings { slot = TimeSlot.Evening,   durationSeconds = 120f },
        new TimeSlotSettings { slot = TimeSlot.Night,     durationSeconds = 90f  },
    };

    [Header("Starting State")]
    public TimeSlot startingSlot = TimeSlot.Morning;

    [Header("Options")]
    [Tooltip("If false, time stops at Night and waits for AdvanceSlot() to be called manually.")]
    public bool loopContinuously = true;
    [Tooltip("If true, time will not advance automatically. Call AdvanceSlot() manually.")]
    public bool manualControl = false;

    // -----------------------------------------------------------------------
    // Events  (wire these up in the Inspector or via code)
    // -----------------------------------------------------------------------

    [Header("Events")]
    public UnityEvent onMorning;
    public UnityEvent onAfternoon;
    public UnityEvent onEvening;
    public UnityEvent onNight;

    // Fires on any slot change, passing the new TimeSlot.
    public event Action<TimeSlot> OnSlotChanged;

    // -----------------------------------------------------------------------
    // Public Read-Only State
    // -----------------------------------------------------------------------

    // The currently active time slot.
    public TimeSlot CurrentSlot { get; private set; }

    // Seconds elapsed within the current slot (0 to slot duration).
    public float SlotTimer { get; private set; }

    // 0-1 progress through the current slot.
    public float SlotProgress => SlotTimer / CurrentSlotDuration;

    // How long the current slot lasts in seconds.
    public float CurrentSlotDuration => GetDuration(CurrentSlot);

    // -----------------------------------------------------------------------
    // Private
    // -----------------------------------------------------------------------

    private int _slotIndex;
    private bool _isPaused;

    // -----------------------------------------------------------------------
    // Unity Lifecycle
    // -----------------------------------------------------------------------

    void Start()
    {
        _slotIndex = SlotToIndex(startingSlot);
        CurrentSlot = startingSlot;
        SlotTimer = 0f;
        FireSlotEvent(CurrentSlot);
        textTime.text = CurrentSlot.ToString();
    }

    void Update()
    {
        if (manualControl || _isPaused) return;

        SlotTimer += Time.deltaTime;

        if (SlotTimer >= CurrentSlotDuration)
        {
            SlotTimer = 0f;
            TryAdvanceSlot();
        }
    }

    // -----------------------------------------------------------------------
    // Public API
    // -----------------------------------------------------------------------

    // Immediately jump to a specific slot and reset its timer.
    // Fires slot events as normal.
    public void SetSlot(TimeSlot slot)
    {
        _slotIndex = SlotToIndex(slot);
        CurrentSlot = slot;
        SlotTimer = 0f;
        FireSlotEvent(CurrentSlot);
        textTime.text = CurrentSlot.ToString();
    }

    // Manually advance to the next slot.
    // Useful when loopContinuously = false, or for skip/fast-forward mechanics.
    public void AdvanceSlot()
    {
        SlotTimer = 0f;
        TryAdvanceSlot();
    }

    // Pause or resume automatic time advancement.
    public void SetPaused(bool paused) => _isPaused = paused;

    // Toggle pause state.
    public void TogglePause() => _isPaused = !_isPaused;

    // Returns true if the given slot is currently active.
    public bool Is(TimeSlot slot) => CurrentSlot == slot;

    // Returns true during Morning or Afternoon.
    public bool IsDay => CurrentSlot == TimeSlot.Morning || CurrentSlot == TimeSlot.Afternoon;

    // Returns true during Evening or Night.
    public bool IsNight => CurrentSlot == TimeSlot.Evening || CurrentSlot == TimeSlot.Night;

    // -----------------------------------------------------------------------
    // Private Helpers
    // -----------------------------------------------------------------------

    private void TryAdvanceSlot()
    {
        int nextIndex = (_slotIndex + 1) % slots.Length;

        // If we've wrapped around and looping is off, stop at the last slot.
        if (!loopContinuously && nextIndex == 0)
        {
            SlotTimer = CurrentSlotDuration;
            return;
        }

        _slotIndex = nextIndex;
        CurrentSlot = slots[_slotIndex].slot;
        FireSlotEvent(CurrentSlot);
    }

    private void FireSlotEvent(TimeSlot slot)
    {
        OnSlotChanged?.Invoke(slot);

        switch (slot)
        {
            case TimeSlot.Morning: onMorning?.Invoke(); break;
            case TimeSlot.Afternoon: onAfternoon?.Invoke(); break;
            case TimeSlot.Evening: onEvening?.Invoke(); break;
            case TimeSlot.Night: onNight?.Invoke(); break;
        }
    }

    private float GetDuration(TimeSlot slot)
    {
        foreach (var s in slots)
            if (s.slot == slot) return s.durationSeconds;

        return 120f;
    }

    private int SlotToIndex(TimeSlot slot)
    {
        for (int i = 0; i < slots.Length; i++)
            if (slots[i].slot == slot) return i;

        return 0;
    }
}
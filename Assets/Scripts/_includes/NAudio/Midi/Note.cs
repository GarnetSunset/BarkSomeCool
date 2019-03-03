namespace mid2chart {
    public class MIDNote : Event {
        public const int G = 0, R = 1, Y = 2, B = 3, O = 4;
        public int note;

        public MIDNote(int note, long tick, long sus) {
            this.note = note;
            this.tick = tick;
            this.sus = sus;
        }
    }
}
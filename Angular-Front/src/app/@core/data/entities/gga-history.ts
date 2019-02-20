import { SessionHistory } from "./session-history";

export class GGAHistory {
    constructor(
        public ID?: string,
        public Account?: string,
        public AccountType?: string,
        public AccountSYS?: string,
        public FixedTime?: Date, 
        public Lng?: Number,
        public Lat?: Number,
        public Status?: string,
        public GGAInfo?: string,
        public Session?: SessionHistory
    ) { }
}

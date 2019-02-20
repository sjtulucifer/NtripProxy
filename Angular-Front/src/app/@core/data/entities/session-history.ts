import { GGAHistory } from "./gga-history";

export class SessionHistory {
    constructor(
        public ID?: string,
        public AccountName?: string,
        public AccountType?: string,
        public AccountSYSName?: string,
        public MountPoint?: string, 
        public Client?: string,
        public ClientAddress?: string,
        public ConnectionStart?: Date,
        public ConnectionEnd?: Date,
        public GGACount?: Number,
        public FixedCount?: Number,
        public ErrorInfo?: string,
        public GGAHistories?: Array<GGAHistory>
    ) { }
}
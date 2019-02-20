export class AccountSYS {
    constructor(
        public ID?: string,
        public Name?: string,
        public Password?: string,
        public Register?: Date,
        public Expire?: Date, 
        public LastLogin?: Date,
        public Age?: Number,
        public IsOnline?: boolean,
        public IsLocked?: boolean,
        public IsDelete?: boolean
    ) { }
}

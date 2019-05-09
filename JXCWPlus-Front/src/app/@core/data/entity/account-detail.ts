export class AccountDetail {
    constructor(
        public AccountName?: string,
        public AccountPassword?: string,
        public AccountRegister?: Date,
        public AccountExpire?: Date, 
        public AccountIsLocked?: boolean,
    ) { }
}

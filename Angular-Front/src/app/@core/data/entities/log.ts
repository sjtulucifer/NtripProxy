export class Log {
    constructor(
        public ID?: string,
        public Time?: Date,
        //用户ID号
        public User?: string, 
        public Action?: string,
        public Module?: string,
        public Message?: string
    ) { }
}

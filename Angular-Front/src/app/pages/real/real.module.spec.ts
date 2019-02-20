import { RealModule } from './real.module';

describe('RealModule', () => {
  let realModule: RealModule;

  beforeEach(() => {
    realModule = new RealModule();
  });

  it('should create an instance', () => {
    expect(realModule).toBeTruthy();
  });
});

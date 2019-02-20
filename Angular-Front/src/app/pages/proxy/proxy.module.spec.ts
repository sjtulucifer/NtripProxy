import { ProxyModule } from './proxy.module';

describe('ProxyModule', () => {
  let proxyModule: ProxyModule;

  beforeEach(() => {
    proxyModule = new ProxyModule();
  });

  it('should create an instance', () => {
    expect(proxyModule).toBeTruthy();
  });
});

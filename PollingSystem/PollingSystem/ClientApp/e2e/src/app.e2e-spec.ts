// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

import { AppPage } from './app.po';

describe('PollingSystem App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display application title: PollingSystem', async () => {
    await page.navigateTo();
    expect(await page.getAppTitle()).toEqual('PollingSystem');
  });
});

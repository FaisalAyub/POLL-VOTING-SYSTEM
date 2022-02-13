import { PollAppTemplatePage } from './app.po';

describe('PollApp App', function() {
  let page: PollAppTemplatePage;

  beforeEach(() => {
    page = new PollAppTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});

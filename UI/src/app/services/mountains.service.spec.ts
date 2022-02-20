import { TestBed } from '@angular/core/testing';

import { MountainsService } from './mountains.service';

describe('MountainsService', () => {
  let service: MountainsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MountainsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

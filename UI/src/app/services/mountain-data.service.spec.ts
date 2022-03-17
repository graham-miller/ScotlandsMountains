import { TestBed } from '@angular/core/testing';

import { MountainDataService } from './mountain-data.service';

describe('MountainDataService', () => {
  let service: MountainDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MountainDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

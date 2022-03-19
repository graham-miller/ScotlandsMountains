import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Mountain } from 'src/app/models/mountain';
import { MountainDataService } from 'src/app/services/mountain-data.service';

@Component({
  selector: 'mountain',
  templateUrl: './mountain.component.html',
  styleUrls: ['./mountain.component.css']
})
export class MountainComponent implements OnInit {

  constructor(
    private mountainsDataService: MountainDataService,
    private route: ActivatedRoute,
    private router: Router
  ) { }
  
  mountainId?: string;
  mountain?: Mountain;
  isLoading = true;

  ngOnInit(): void {
      this.route.params.subscribe(params => {
        this.mountainId = params['id'];
        this.isLoading = true;
        this.loadMountain();
      });
  }

  private loadMountain() {
    if (this.mountainId) {
      this.mountainsDataService.getMountain(this.mountainId)
        .subscribe(data => {
          this.mountain = data;
          this.isLoading = false;
        });
    }
  }
}

import { formatNumber } from '@angular/common';
import { AfterViewInit, Component, ElementRef, Input, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';

import * as L from 'leaflet';
import GestureHandling from 'leaflet-gesture-handling';
import { MountainSummary } from 'src/app/models/mountain';
import { MetersToFeetPipe } from 'src/app/pipes/meters-to-feet.pipe';

L.Marker.prototype.options.icon = L.divIcon({ className: 'mountain-marker' });
L.Map.addInitHook("addHandler", "gestureHandling", GestureHandling);

@Component({
  selector: 'map',
  template: '<div id="map"></div>',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements AfterViewInit, OnChanges, OnDestroy {

  constructor(
    private metersToFeet: MetersToFeetPipe,
    private elementRef: ElementRef
  ) { }

  @Input() mountains: MountainSummary[] = [];

  private map?: L.Map;
  private markers = L.layerGroup();

  ngAfterViewInit(): void {
    this.initMap();
    this.addMountainsToMap();
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.addMountainsToMap();
  }

  ngOnDestroy(): void {
    this.destroyMap();
  }

  private initMap(): void {
    this.map = L.map('map', {
      center: [56.659406, -4.011214],
      zoom: 7,
      gestureHandling: true,
      attributionControl: false
    } as L.MapOptions);

    this.addBaseLayers();
    this.addMapControls();
  }

  private addBaseLayers() {
    // const outdoors = this.createThunderforestTileLayer('outdoors');
    // const landscape = this.createThunderforestTileLayer('landscape');
    // const transport = this.createThunderforestTileLayer('transport');
    // const cycle = this.createThunderforestTileLayer('cycle');

    // const baseLayers = {
    //   'Outdoors': outdoors,
    //   'Landscape': landscape,
    //   'Transport': transport,
    //   'Cycle': cycle
    // };

    const outdoors = this.createMapboxTileLayer('mapbox/outdoors-v11');
    const street = this.createMapboxTileLayer('mapbox/streets-v11');
    const satellite = this.createMapboxTileLayer('mapbox/satellite-streets-v11');

    const baseLayers = {
      'Outdoors': outdoors,
      'Street': street,
      'Satellite': satellite
    };

    const markers = L.layerGroup([this.markers]);

    if (this.map) { 
      L.control.layers(baseLayers, { 'Markers': markers }).addTo(this.map);

      outdoors.addTo(this.map);
      markers.addTo(this.map);
    }
  }

  private createThunderforestTileLayer(moniker: string): L.TileLayer {
    return L.tileLayer(
      `https://tile.thunderforest.com/${moniker}/{z}/{x}/{y}.png?apikey=231e70bf20b64d8ca94199922441d3f7`,
      { maxZoom: 18 });
  }

  private createMapboxTileLayer(moniker: string): L.TileLayer {
    const accessToken = 'pk.eyJ1IjoiZ3JhaGFtbSIsImEiOiJjbDE5cjd4NGcxYzkzM2JzMTNvdzZ0M2IyIn0.yyOxzlYe6HoJ_HL8OQho2g';
    const attribution =
      '© <a href="https://www.mapbox.com/about/maps/">Mapbox</a> ' +
      '© <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a> ' +
      '<a href="https://www.mapbox.com/map-feedback/" target="_blank">Improve this map</a>';

    return L.tileLayer(
      'https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
      attribution: attribution,
      tileSize: 512,
      maxZoom: 18,
      zoomOffset: -1,
      id: moniker,
      accessToken: accessToken
    });
  }




  private addMapControls() {
    if (this.map) { 

      L.control.attribution({
        prefix: '',
        position: 'bottomleft'
      }).addTo(this.map);

      L.control.scale({
        maxWidth: 200,
        metric: true,
        imperial: true,
        updateWhenIdle: false,
        position: 'bottomright'
      }).addTo(this.map);
    }
  }

  private addMountainsToMap() {
    this.removeMountainsFromMap();
    const group: L.LayerGroup = this.markers;
    this.mountains.forEach((m, i) => group.addLayer(this.getMarker(m, -i)));

    if (this.map) {
      this.map.flyToBounds(this.getBounds(this.mountains));
    }
  }

  private removeMountainsFromMap() {
    this.markers.clearLayers();
  }

  private getMarker(mountain: MountainSummary, zIndex: number): L.Marker {
    let marker = L.marker([mountain.latitude, mountain.longitude], { zIndexOffset: zIndex });
    marker.on('mouseover', e => this.showPopup(e, mountain));
    marker.on('mouseout', e => this.hidePopup(e));
    marker.on('click', e => this.togglePopup(e, mountain));

    return marker;
  }

  private togglePopup(event: L.LeafletEvent, mountain: MountainSummary) {
    if (event.target.getPopup()) {
      this.hidePopup(event);
    } else {
      this.showPopup(event, mountain);
    }
  }

  private showPopup(event: L.LeafletEvent, mountain: MountainSummary) {
    event.target.bindPopup(this.getPopupText(mountain)).openPopup();
  }

  private hidePopup(event: L.LeafletEvent) {
    event.target.closePopup();
    event.target.unbindPopup();
  }

  private getPopupText(mountain: MountainSummary): string {
    const meters = formatNumber(mountain.height, 'en-GB', '1.0-0');
    const feet = formatNumber(this.metersToFeet.transform(mountain.height), 'en-GB', '1.0-0');
    return `<span>${mountain.name}<br/>${meters}m (${feet}ft)</span>`    
  }

  private getBounds(mountains: MountainSummary[]): L.LatLngBoundsExpression {
    let minLat: number = 90;
    let maxLat: number = -90;
    let minLng: number = 180;
    let maxLng: number = -180;

    mountains.forEach(m => {
      minLat = m.latitude < minLat ? m.latitude : minLat;
      maxLat = m.latitude > maxLat ? m.latitude : maxLat;
      minLng = m.longitude < minLng ? m.longitude : minLng;
      maxLng = m.longitude > maxLng ? m.longitude : maxLng;
    })

    return [[minLat, minLng], [maxLat, maxLng]]
  }

  private destroyMap() {
    if (this.map) { 
      this.map.off();
      this.map.remove();
      this.map.getContainer().replaceWith(this.elementRef.nativeElement.innerHTML);
    }
  }
}

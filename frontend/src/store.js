import { create } from "zustand";

const useFormData = create((set) => ({
  randomData: null,
  setRandomData: (data) => set((store) => ({ ...store, randomData: data })),
  response: null,
  setResponse: (response) => set((store) => ({ ...store, response })),
  mode: { name: "Modes", link: "mode1" },
  changeMode: (mode) => set((store) => ({ ...store, mode })),
  formDataToSend: {
    capacity: 0,
    distances: [],
    values: [],
    weights: [],
  },
  setFormDataToSend: (formData) =>
    set((store) => ({
      ...store,
      formDataToSend: { ...store.formDataToSend, ...formData },
    })),
  changeSettings: (settings) =>
    set((store) => ({
      ...store,
      formDataToSend: {
        ...store.formDataToSend,
        settings: {
          ...store.formDataToSend.settings,
          ...settings,
        },
      },
    })),
  formDataStorage: {
    capacity: 0,
    objects_count: 0,
    locations_count: 0,
    trucks_count: 0,
  },
  setFormDataStorage: (data) =>
    set((store) => ({
      ...store,
      formDataStorage: { ...store.formDataStorage, ...data },
    })),
  addTruckCapacities: (capacities) =>
    set((store) => ({
      ...store,
      formDataStorage: { ...store.formDataStorage, capacities },
    })),
  addObjects: (objects) =>
    set((store) => ({
      ...store,
      formDataStorage: { ...store.formDataStorage, objects },
    })),
  addLocations: (locations_name) =>
    set((store) => ({
      ...store,
      formDataStorage: {
        ...store.formDataStorage,
        locations: { locations_name },
      },
    })),
  addLoctaionsTime: (distances) =>
    set((store) => ({
      ...store,
      formDataStorage: {
        ...store.formDataStorage,
        locations: {
          ...store.formDataStorage.locations,
          distances,
        },
      },
    })),
  addStartEndIndexesTrucks: (startEnd) =>
    set((store) => ({
      ...store,
      formDataStorage: {
        ...store.formDataStorage,
        indicesofstartingpointes: startEnd["indicesofstartingpointes"],
        indicesofendingpointes: startEnd["indicesofendingpointes"],
      },
    })),
  addStartEndIndexesObjects: (startEnd) =>
    set((store) => ({
      ...store,
      formDataStorage: {
        ...store.formDataStorage,
        indicesofpickinguppoints: startEnd["indicesofpickinguppoints"],
        indicesofdroppingoffpoints: startEnd["indicesofdroppingoffpoints"],
      },
    })),
  addPickUpDropOffTime: (pickUpDropOff) =>
    set((store) => ({
      ...store,
      formDataStorage: {
        ...store.formDataStorage,
        pickUpPenalties: pickUpDropOff["pickUpPenalties"],
        dropOffPenalties: pickUpDropOff["dropOffPenalties"],
      },
    })),
}));

export default useFormData;

import { create } from "zustand";

const useFormData = create((set) => ({
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
      formDataToSend: formData,
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
}));

export default useFormData;

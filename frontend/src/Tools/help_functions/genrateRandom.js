const getRandomInt = (min, max) =>
  Math.floor(Math.random() * (max - min + 1)) + min;
const getRandomArray = (length, min, max) =>
  Array.from({ length }, () => getRandomInt(min, max));

const getRandom2DArrayWithZeroRadius = (size, min, max) => {
  const array = Array.from({ length: size }, () => Array(size).fill(0));
  for (let i = 0; i < size; i++) {
    for (let j = i + 1; j < size; j++) {
      const randomValue = getRandomInt(min, max);
      array[i][j] = randomValue;
      array[j][i] = randomValue;
    }
  }
  return array;
};

export const generateRandomData = (mode) => {
  const isModeThreeOrFour = mode === "mode3" || mode === "mode4";
  const numOfItems = getRandomInt(1, 10);
  const numOfTrucks = getRandomInt(1, 10);
  const formDataStorage = {
    capacity: getRandomInt(100, 700),
    capacities: getRandomArray(numOfTrucks, 100, 700),
    distances: getRandom2DArrayWithZeroRadius(numOfTrucks, 0, 10000),
    objects: {
      values: getRandomArray(
        isModeThreeOrFour ? numOfTrucks : numOfItems,
        1,
        100
      ),
      weights: getRandomArray(
        isModeThreeOrFour ? numOfTrucks : numOfItems,
        1,
        100
      ),
    },
    indicesOfStartingPoints: getRandomArray(numOfTrucks, 0, numOfTrucks),
    indicesOfEndingPoints: getRandomArray(numOfTrucks, 0, numOfTrucks),
    indicesOfPickingUpPoints: getRandomArray(numOfItems, 0, numOfItems),
    indicesOfDroppingOffPoints: getRandomArray(numOfItems, 0, numOfItems),
    pickUpPenalties: getRandomArray(numOfItems, 0, 50),
    dropOffPenalties: getRandomArray(numOfItems, 0, 50),
  };

  return {
    [mode === "mode1" || mode === "mode2" ? "capacity" : "capacities"]:
      mode === "mode1" || mode === "mode2"
        ? formDataStorage.capacity
        : formDataStorage.capacities,
    distances: formDataStorage.distances,
    values: formDataStorage.objects.values,
    weights: formDataStorage.objects.weights,
    ...(mode !== "mode1" &&
      mode !== "mode2" &&
      mode !== "mode3" && {
        indicesofstartingpointes: formDataStorage.indicesofstartingpointes,
        indicesofendingpointes: formDataStorage.indicesofendingpointes,
      }),
    ...((mode === "mode5" || mode === "mode6") && {
      indicesofpickinguppoints: formDataStorage.indicesofpickinguppoints,
      indicesofdroppingoffpoints: formDataStorage.indicesofdroppingoffpoints,
    }),
    ...(mode === "mode6" && {
      pickUpPenalties: formDataStorage.pickUpPenalties,
      dropOffPenalties: formDataStorage.dropOffPenalties,
    }),
  };
};

import { Route, Routes, useLocation } from "react-router-dom";
import Footer from "./layout/Footer";
import NavBar from "./layout/NavBar";
import CapacityForm from "./pages/Form/CapacityForm";
import DistancesForm from "./pages/Form/DistancesForm";
import FinalPage from "./pages/Form/FinalPage";
import LocationsForm from "./pages/Form/LocationsForm";
import ObjectForm from "./pages/Form/ObjectsForm";
import Page1 from "./pages/Form/Page1";
import Nodes from "./pages/Map/Nodes";

function App() {
  const { pathname } = useLocation();

  return (
    <div className="App h-full p-4 flex flex-col gap-3">
      {pathname !== "/map/nodes" && <NavBar />}
      <div
        className={`flex justify-center items-center ${
          pathname !== "/map/nodes" ? "h-[80vh]" : "h-[100vh]"
        } `}>
        <Routes>
          <Route path="/" element={<Page1 />} />
          <Route path="page-2" element={<ObjectForm />} />
          <Route path="page-3" element={<LocationsForm />} />
          <Route path="page-4" element={<DistancesForm />} />
          <Route path="capacities" element={<CapacityForm />} />
          <Route path="final" element={<FinalPage />} />
          <Route path="map">
            <Route path="/map/nodes" element={<Nodes />} />
          </Route>
        </Routes>
      </div>
      {pathname !== "/map/nodes" && <Footer />}
    </div>
  );
}

export default App;

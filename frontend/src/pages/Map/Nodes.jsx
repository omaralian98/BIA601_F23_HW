import React, { useEffect, useRef, useState } from "react";
import * as d3 from "d3";
import useFormData from "../../store";
import { FaArrowLeft, FaArrowRight } from "react-icons/fa6";
import { Link } from "react-router-dom";

const Maps = () => {
  const { response, mode, setFormDataToSend } = useFormData();
  useEffect(() => {
    setFormDataToSend({
      distances: [],
      values: [],
      weights: [],
    });
  }, []);
  const multipleMaps =
    mode.link === "mode1" || mode.link === "mode2"
      ? [response.bestRoute]
      : response.trucks;

  return (
    <div className="flex flex-wrap gap-7 w-screen h-screen p-4">
      <Link
        to="/"
        className=" bg-yellow-400 fixed top-5 right-5 rounded-full p-1">
        <FaArrowRight size={40} color="white" />
      </Link>
      {multipleMaps?.map((map, index) => (
        <Graph
          key={index}
          map={map?.route || map}
          includedItems={map.includedItems}
          totalValue={map.value}
          totalDistance={map.distance}
        />
      ))}
    </div>
  );
};

export default Maps;

export const Graph = ({ map, includedItems, totalDistance, totalValue }) => {
  const [windowWidth, setWindowWidth] = useState(window.innerWidth);
  const [windowHeight, setWindowHeight] = useState(window.innerHeight);

  useEffect(() => {
    const handleResize = () => {
      setWindowWidth(window.innerWidth);
      setWindowHeight(windowHeight);
    };

    window.addEventListener("resize", handleResize);
    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, [windowWidth, windowHeight]);

  const { formDataStorage } = useFormData();
  const [nodes, setNodes] = useState([]);
  const [links, setLinks] = useState([]);
  const svgRef = useRef(null);
  let locations = [];
  let temp2 = [];

  const itemNames =
    formDataStorage?.objects?.names.length > 0
      ? includedItems.map((e) => formDataStorage?.objects?.names[parseInt(e)])
      : includedItems;

  const nodesData = formDataStorage?.locations?.locations_name?.map((e) => ({
    id: e,
    label: e,
  }));

  for (let i = 0; i < map?.length; i++) {
    let start;
    let end;
    start = formDataStorage?.locations?.locations_name[parseInt(map[i])];
    end = formDataStorage?.locations?.locations_name[parseInt(map[i + 1])];
    locations.push({ start, end });
  }

  locations.forEach((location) => {
    formDataStorage?.locations?.distances?.forEach((e) => {
      e.forEach((d) => {
        if (
          d.start === location.start &&
          d.end === (location.end ? location.end : location.start)
        ) {
          temp2.push({
            source: d.start,
            target: d.end,
            distance: d.distance,
          });
        }
      });
    });
  });

  useEffect(() => {
    const nodes = nodesData.map((node) => ({ id: node.id, ...node }));
    const links = temp2.map((link) => ({
      source: nodes.find((n) => n.id === link.source),
      target: nodes.find((n) => n.id === link.target),
      distance: link.distance,
    }));

    const simulation = d3
      .forceSimulation(nodes)
      .force(
        "link",
        d3
          .forceLink(links)
          .distance((d) => d.distance)
          .id((d) => d.id)
      )
      .force("charge", d3.forceManyBody().strength(-300))
      .force("center", d3.forceCenter(windowWidth / 2, windowHeight / 2))
      .on("tick", () => {
        setNodes([...nodes]);
        setLinks([...links]);
      });

    return () => {
      simulation.stop();
    };
  }, [nodesData, temp2]);

  return (
    <div className="flex flex-wrap p-3 justify-center items-center overflow-auto w-full h-full border-2 border-black bg-gray-100">
      <div className="w-full flex gap-5 justify-around">
        <div className=" bg-slate-200 rounded-xl text-center p-3">
          <h1 className="text-2xl font-bold">Included items:</h1>
          <div className="flex gap-3">
            {` { `}
            {itemNames.map((e) => (
              <p className="text-lg font-semibold">{e}</p>
            ))}
            {` } `}
          </div>
        </div>

        <div className=" bg-slate-200 rounded-xl text-center p-3">
          <h1 className="text-2xl font-bold">Total distance:</h1>
          <div className="flex gap-3">{`{ ${totalDistance} }`}</div>
        </div>

        <div className=" bg-slate-200 rounded-xl text-center p-3">
          <h1 className="text-2xl font-bold">Total value:</h1>
          <div className="flex gap-3">{`{ ${totalValue} }`}</div>
        </div>
      </div>
      <svg
        ref={svgRef}
        width={windowWidth}
        height={windowHeight}
        className="overflow-auto my-auto">
        <defs>
          <marker
            id="arrowhead"
            markerWidth="10"
            markerHeight="10"
            refX="15"
            refY="3"
            orient="auto"
            markerUnits="strokeWidth">
            <path d="M0,0 L0,6 L9,3 z" fill="black" />
          </marker>
        </defs>
        {links.map((link, index) => (
          <line
            key={index}
            x1={link.source.x}
            y1={link.source.y}
            x2={link.target.x}
            y2={link.target.y}
            stroke="black"
            markerEnd="url(#arrowhead)"
          />
        ))}
        {nodes.map((node, index) => (
          <g key={index}>
            <circle cx={node.x} cy={node.y} r={7} fill={` red`} />
            <text
              x={node.x}
              y={node.y}
              dy="-1em"
              textAnchor="middle"
              fontSize="8px"
              fontWeight="bold"
              fill="black">
              {node.label}
            </text>
          </g>
        ))}
      </svg>
    </div>
  );
};
